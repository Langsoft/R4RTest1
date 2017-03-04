using System;
using System.Text;

/// <summary>
/// This class represnts a R4R Test Client
/// </summary>
public class Client
{
    #region Public Fields
    public int ClientID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    #endregion
    #region Constructor
    public Client()
	{
	}
    #endregion
    #region Methods
    /// <summary>
    /// overrriden ToString Method 
    /// to return a printable representation of this class
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Id: " + ClientID + "\r\n");
        sb.Append("First Name: " + FirstName + "\r\n");
        sb.Append("Last Name: " + LastName  + "\r\n");
        sb.Append("Email: " + Email  + "\r\n");
        sb.Append("Country: " + Country);

        return sb.ToString();
    }
    #endregion
}
