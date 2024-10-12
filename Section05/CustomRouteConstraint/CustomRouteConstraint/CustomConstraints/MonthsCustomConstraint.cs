
using System.Text.RegularExpressions;

namespace CustomRouteConstraint.CustomConstraints
{
    //Ex: sales-report/2030/apr
    public class MonthsCustomConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey)) {
                return false;
            }

            Regex monthRegex = new Regex($"^(apr|jul|oct|jan)$");
            string? monthValue = Convert.ToString(values[routeKey]);

            if (monthRegex.IsMatch(monthValue))
            {
                return true;
            }
            return false;
        }
    }
}
