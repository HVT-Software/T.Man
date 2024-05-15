namespace DR.Domain.Extentions;

public static class NGuidExtention {
    public static Guid NewIfNull(Guid? existedId) {
        return existedId == null ? Guid.NewGuid() : existedId.Value;
    }
}
