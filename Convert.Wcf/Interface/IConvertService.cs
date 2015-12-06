using System.Runtime.Serialization;
using System.ServiceModel;
using Convert.Model;
using Convert.Wcf.DataContract;

namespace Convert.Wcf.Interface
{
    public interface IConvertService
    {
        ChequeModel Translate(ChequeDataModel model);
    }
}
