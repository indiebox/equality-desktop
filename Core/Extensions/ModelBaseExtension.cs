using System.Linq;

using Catel.Data;

namespace Equality.Core.Extensions
{
    public static class ModelBaseExtension
    {
        /// <summary>
        /// Synchronize model with another.
        /// </summary>
        /// <param name="this">The model to be synchronized.</param>
        /// <param name="source">The source model.</param>
        public static void SyncWith(this ModelBase @this, ModelBase source)
        {
            var sourceProps = source.GetType().GetProperties().Where(x => x.CanRead).ToList();
            var destProps = @this.GetType().GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps) {
                var property = destProps.FirstOrDefault(x => x.Name == sourceProp.Name);

                if (property is not null) {
                    property.SetValue(@this, sourceProp.GetValue(source, null), null);
                }
            }
        }
    }
}
