using AutoMapper;

namespace Discount.Application.Mappers
{
    internal class DiscountMapper
    {
        private static readonly Lazy<IMapper> lazyMapper = new Lazy<IMapper>(valueFactory: () => {

            var configuration = new MapperConfiguration(configuration =>
            {
                configuration.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                configuration.AddProfile<DiscountProfile>();
            });
            var mapper = configuration.CreateMapper();
            return mapper;
        });

        public static IMapper MapperExt => lazyMapper.Value;
    }
}
