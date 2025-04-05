namespace TaskManagerCourse.Api.Abstractions
{
    public abstract class AbsrtactServices
    {
        public bool DoAction(Action action)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
