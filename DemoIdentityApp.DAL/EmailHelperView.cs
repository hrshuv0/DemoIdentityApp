namespace DemoIdentityApp.DAL;

public static class EmailHelperView
{
    public static string NewLogin  = "<div style=\"background-color: rgb(35, 199, 207); margin-left: 15%;margin-right: 15%;padding: 10px; text-align: center; border-radius: 2%;\">" +
                                     "<h1 style=\"color: rgb(8, 91, 102);\">TeleDoc BD</h1>" +
                                     "<h1 style=\"color: rgba(9, 13, 96, 0.533);\">Hey! New login to your account noticed</h1>" +
                                     $"<p>We noticed a new sign-in to your TeleDoc Account at {DateTime.Now}</p>" +
                                     "<p>If this was you, you don’t need to do anything. If not, Take action to secure your account. </p></div>\"";
    
    public static string Welcome = "Welcome";

    
}