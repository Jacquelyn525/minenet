using MvcWeb.Paradox;

namespace MvcWeb.Models;


[TransientService]
public class TagHistoryContext : ITagHistoryContext {

  private readonly DbAdapter _dbAdapter = null;



  public TagHistoryContext(DbAdapter dbAdapter) {
    _dbAdapter = dbAdapter;
  }



}
