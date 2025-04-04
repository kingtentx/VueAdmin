namespace VueAdmin.Api.Dtos
{
    [Serializable]
    public class ListResultDto<T> : IListResult<T>
    {
        private IReadOnlyList<T>? _items;

        public IReadOnlyList<T> Items
        {
            get
            {
                return _items ?? (_items = new List<T>());
            }
            set
            {
                _items = value;
            }
        }

        //
        // 摘要:
        //     Creates a new Volo.Abp.Application.Dtos.ListResultDto`1 object.
        public ListResultDto()
        {
        }

        //
        // 摘要:
        //     Creates a new Volo.Abp.Application.Dtos.ListResultDto`1 object.
        //
        // 参数:
        //   items:
        //     List of items
        public ListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }

    public interface IListResult<T>
    {
        //
        // 摘要:
        //     List of items.
        IReadOnlyList<T> Items { get; set; }
    }
}
