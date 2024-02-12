using Core.Entity;

namespace Core.DapperRepository;

public class DapperExtention<TEntity> where TEntity : BaseEntity
{
    public enum ActionExcute
    {
        Add,
        Get,
        Update
    }

    public string GetPropName(ActionExcute action)
    {
        var sql = "";
        switch (action)
        {
            case ActionExcute.Add:
                foreach (var propertyInfo in typeof(TEntity).GetProperties()) sql += '@' + propertyInfo.Name + ',';
                break;
            case ActionExcute.Get:
                foreach (var propertyInfo in typeof(TEntity).GetProperties()) sql += propertyInfo.Name + ',';
                break;
            case ActionExcute.Update:
                foreach (var propertyInfo in typeof(TEntity).GetProperties())
                    sql += propertyInfo.Name + "= @" + propertyInfo.Name + ',';
                break;
        }

        return sql;
    }
}