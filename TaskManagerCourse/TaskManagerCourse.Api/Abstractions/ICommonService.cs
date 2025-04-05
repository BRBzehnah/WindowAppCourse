namespace TaskManagerCourse.Api.Obstractions
{
    public interface ICommonService<T>
    {
        bool Create(T model);

        bool Update(T model, int id);

        bool Delete(int id);

    }
}
