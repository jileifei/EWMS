namespace CMS.CommonLib.Extension
{
    public static class ModelStateExtensions
    {
        public static string ExpendErrors(this System.Web.Mvc.Controller controller)
        {
            System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();
            foreach (var item in controller.ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    for (int i = item.Errors.Count - 1; i >= 0; i--)
                    {
                        sbErrors.Append(item.Errors[i].ErrorMessage);
                        sbErrors.Append("<br/>");
                    }
                }
            }
            return sbErrors.ToString();
        }
    }
}
