namespace Verisoft.Core.AspNet.Mvc
{
    public class BadRequestResponse
    {
        public string Code { get; set; }

        public string Text { get; set; }

        public object Parameters { get; set; }
    }
}