namespace NBD4.ViewModels
{
	/// <summary>
	/// Interface for my own email service
	/// </summary>
	public interface IMyEmailSender
	{
		Task SendOneAsync(string name, string email, string subject, string htmlMessage);
		Task SendToManyAsync(EmailMessage emailMessage);
	}
}
