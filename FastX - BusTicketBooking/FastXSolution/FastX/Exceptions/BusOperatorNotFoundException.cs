using System.Runtime.Serialization;

namespace FastX.Exceptions
{
    [Serializable]
    public class BusOperatorNotFoundException : Exception
    {
        string message;
        public BusOperatorNotFoundException()
        {
            message = "BusOperator not found";
        }
        public override string Message => message;


    }
}