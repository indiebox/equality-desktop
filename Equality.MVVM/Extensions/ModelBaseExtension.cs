using System.Linq;

using Catel.Data;

namespace Equality.Extensions
{
    public static class ModelBaseExtension
    {
        /// <summary>
        /// Synchronize model with another.
        /// </summary>
        /// <param name="this">The model to be synchronized.</param>
        /// <param name="source">The source model.</param>
        public static void SyncWith<TModelBase>(this TModelBase @this, TModelBase source)
            where TModelBase : ModelBase
        {
            var sourceProps = source.GetType().GetProperties()
                .Where(x => x.CanRead)
                .ToList();
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

        /// <summary>
        /// Synchronize specified properties of model with another.
        /// </summary>
        /// <param name="this">The model to be synchronized.</param>
        /// <param name="source">The source model.</param>
        /// <param name="properties">The properties.</param>
        public static void SyncWithOnly<TModelBase>(this TModelBase @this, TModelBase source, params string[] properties)
            where TModelBase : ModelBase
        {
            var sourceProps = source.GetType().GetProperties()
                .Where(x => x.CanRead && properties.Contains(x.Name))
                .ToList();
            var destProps = @this.GetType().GetProperties()
                .Where(x => x.CanWrite && properties.Contains(x.Name))
                .ToList();

            foreach (var sourceProp in sourceProps) {
                var property = destProps.FirstOrDefault(x => x.Name == sourceProp.Name);

                if (property is not null) {
                    property.SetValue(@this, sourceProp.GetValue(source, null), null);
                }
            }
        }

        /// <summary>
        /// Synchronize all properties that not in specified ones of model with another.
        /// </summary>
        /// <param name="this">The model to be synchronized.</param>
        /// <param name="source">The source model.</param>
        /// <param name="properties">The properties to exclude.</param>
        public static void SyncWithExcept<TModelBase>(this TModelBase @this, TModelBase source, params string[] properties)
            where TModelBase : ModelBase
        {
            var sourceProps = source.GetType().GetProperties()
                .Where(x => x.CanRead && !properties.Contains(x.Name))
                .ToList();
            var destProps = @this.GetType().GetProperties()
                .Where(x => x.CanWrite && !properties.Contains(x.Name))
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
