namespace MyTime.Common
{
   public interface IProcessInformation
   {
      string FriendlyName { get; }
      string ProcessDescription { get; }
      string ProcessName { get; }
   }
}