# Use a imagem base do SQL Server
FROM mcr.microsoft.com/mssql/server:2019-latest

# Trocar para o usuário root para instalar pacotes
USER root

# Instalar pacotes necessários
RUN apt-get update && \
    apt-get install -y curl apt-transport-https gnupg && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev && \
    apt-get clean

# Verificar se o grupo e usuário mssql existem, se não, criá-los
RUN getent group mssql || groupadd -g 10001 mssql && \
    getent passwd mssql || useradd -r -u 10001 -g mssql mssql

# Voltar para o usuário mssql após a instalação dos pacotes
USER mssql

# Adicionar o diretório dos scripts SQL
COPY ./src/Infrastructure/Data/Migrations/Init /var/opt/mssql/init-sql

# Ajustar permissões para o diretório e scripts
USER root
RUN chown -R mssql:mssql /var/opt/mssql/init-sql

# Voltar para o usuário mssql
USER mssql

# Comando para iniciar o SQL Server e executar os scripts SQL
CMD /bin/bash -c "/opt/mssql/bin/sqlservr & \
    echo 'Waiting SQL Server to start...'; \
    sleep 10; \
    echo 'SQL Server started. Executing scripts...'; \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'YourStrong!Passw0rd' -i /var/opt/mssql/init-sql/DataBase.sql && \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'YourStrong!Passw0rd' -i /var/opt/mssql/init-sql/Down.sql && \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'YourStrong!Passw0rd' -i /var/opt/mssql/init-sql/Up.sql && \
    echo 'Scripts executed successfully!'; \
    sleep infinity"