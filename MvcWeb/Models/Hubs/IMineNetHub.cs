namespace MvcWeb.Models.Hubs;

public interface IMineNetHub {
  Task AlertNotification(IAlert alertUpdate);
  Task LocationNotification(ILocationUpdate locationUpdate);
}
