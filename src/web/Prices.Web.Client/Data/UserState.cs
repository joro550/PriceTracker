using BlazorState;

namespace Prices.Web.Client.Data
{
    public class UserState : State<UserState>
    {
        public string Token { get; set; }
        
        public override object Clone() => new UserState
        {
            Token =  Token
        };

        protected override void Initialize() 
            => Token = string.Empty;
    }
}