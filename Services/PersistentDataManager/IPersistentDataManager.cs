using WPFTodo.Models;

namespace WPFTodo.Services.Provider;
public interface IPersistentDataManager
{
    public PersistentData? GetPersistentData();

    public void SaveData(PersistentData data);
}
