using System.Text.Json.Serialization;

namespace Application.Common.Outputs
{
    public class BaseOutput
    {
        private readonly List<string> _errors = new();

        public IReadOnlyList<string> Errors => _errors;

        [JsonIgnore]
        public bool IsValid => !_errors.Any();

        public void AddError(string errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                _errors.Add(errorMessage);
            }
        }

        public void AddErrors(IEnumerable<string> errorMessages)
        {
            if (errorMessages != null)
            {
                _errors.AddRange(errorMessages.Where(msg => !string.IsNullOrWhiteSpace(msg)));
            }
        }

        public void ClearErrors()
        {
            _errors.Clear();
        }
    }
}
