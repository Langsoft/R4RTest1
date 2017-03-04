using System;
using System.Text;

public class Client
{
    public int ClientID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    
    
    public Client()
	{
	}

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Id: " + ClientID + "\r\n");
        sb.Append("First Name: " + FirstName + "\r\n");
        sb.Append("Last Name: " + LastName  + "\r\n");
        sb.Append("Email: " + Email  + "\r\n");
        sb.Append("Country " + Country + "\r\n\r\n");

        return sb.ToString();
    }

}
