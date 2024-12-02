using System.Linq.Expressions;

namespace Verisoft.Core.Common.Linq.Specifications
{
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression @from;

        private readonly Expression to;

        public ReplaceExpressionVisitor(Expression from, Expression to)
        {
            this.@from = from;
            this.to = to;
        }

        public override Expression Visit(Expression node)
        {
            return node == @from ? to : base.Visit(node);
        }
    }
}