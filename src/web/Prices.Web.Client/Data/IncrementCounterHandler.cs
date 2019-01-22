using System.Threading;
using System.Threading.Tasks;
using BlazorState;

namespace Prices.Web.Client.Data
{
    public class IncrementCounterHandler 
        : RequestHandler<LoginRequest, UserState>
    {
        private readonly IStore _aStore;
        
        public IncrementCounterHandler(IStore aStore) 
            : base(aStore)
        {
            _aStore = aStore;
        }

        public override Task<UserState> Handle(
            LoginRequest aIncrementCounterRequest, 
            CancellationToken aCancellationToken)
        {
            var currentState = _aStore.GetState<UserState>();
            currentState.Token = aIncrementCounterRequest.UserToken;
            return Task.FromResult(currentState);
        }
    }
}