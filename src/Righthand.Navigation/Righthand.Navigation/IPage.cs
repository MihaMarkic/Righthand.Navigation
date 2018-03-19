using System.Threading.Tasks;

namespace Righthand.Navigation
{
    /// <summary>
    /// Represents a navigation page.
    /// </summary>
    /// <typeparam name="T">Type of page.</typeparam>
    public interface IPage<T>
    {
        /// <summary>
        /// This method gets called prior to navigation to another page.
        /// </summary>
        /// <param name="to">An instance of next page.</param>
        /// <returns>Returns true when navigation is allowed, false otherwise.</returns>
        ValueTask<bool> CanNavigate(T to);
        /// <summary>
        /// This method is called when page is no longer on the stack. Do the cleaning here.
        /// </summary>
        /// <remarks>
        /// When IoC is used, Dispose might be called from IoC framework, not directly.
        /// This differently named method solves it so the class can be used either with IoC framework or independently.
        /// </remarks>
        void Removed();
    }
}
