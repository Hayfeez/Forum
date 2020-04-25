using System;
namespace Forum.Helpers
{
    public static class BaseClass
    {

        public static string GetResponseMessage(DbActionsResponse response, DbActions action, string entity = null)
        {
            switch (response)
            {
                //case DbActionsResponse.Success:
                //    if (action == DbActions.Add)
                //        return $"{entity} saved successfully";
                //    else if(action == DbActions.Update)
                //        return $"{entity} updated successfully";
                //    else
                //        return $"{entity} deleted successfully";
                case DbActionsResponse.Success:
                    return "Record saved successfully";
                case DbActionsResponse.NotFound:
                    return "Item not found";
                case DbActionsResponse.DuplicateExist:
                    return "Record could not be saved because duplicate item exists";
                case DbActionsResponse.DeleteDenied:
                    return "Record could not be deleted";
                case DbActionsResponse.Error:
                    return "An error occured";
                default:
                    return "";
            }
        }

    }


    public enum DbActions
    {
        Add = 1,
        Update,
        Delete
    }

    public enum DbActionsResponse
    {
        Success = 1,
        Failed,
        NotFound,
        DuplicateExist,
        DeleteDenied,
        Error
    }

    public enum Status
    {
        Success = 1,
        Failed,
        NotFound,
        Error
    }

    public class ResponseObject
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }


    public class ReadAppSettings
    {
        public string SMTPServerHost { get; set; }
        public string SMTPSenderAddress { get; set; }
        public string SMTPDisplayAddress { get; set; }
        public string SMTPServerUsername { get; set; }
        public string SMTPServerPassword { get; set; }
        public int SMTPServerPort { get; set; }
    }

    public class Tags
    {
        public string Text { get; set; }
        public string ColourCode  { get; set; }
    }
  
}

