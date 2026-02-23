namespace Domain.Enum;

public enum AccountTypeStatus
{
    Seller,
    Buyer,
}

public enum ItemStatus
{
    Active,
    Sold,
    Removed,
}

public enum OrderComfirmStatus
{
    Pending,
    Confirmed,
    Cancelled,
}

// public enum ImgOwnerTypeStatus
// {
//     Profile,
//     Post,
// }

public enum BuyerCondition
{
    Refund,
    Norefund,
}

public enum ItemCondition
{
    New,
    likeNew,
    Used,
    error,
}

