namespace DynamicsValue.AzFunctions
{
    public class GenericResult
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }

        public static GenericResult Succeed() 
        {
            return new GenericResult() 
            {
                Succeeded = true,
                ErrorMessage = ""
            };
        }
    }
}
