using System.Runtime.Serialization;
using System.ServiceModel;
namespace Convert.Wcf.DataContract
{
    [DataContract]
    public class ChequeDataModel
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public double Amount { get; set; }

    }
}