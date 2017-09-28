using System;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace ClassLibrary1
{
public class FeedMessagesMapping : ClassMap<FeedMessage>
{
    public FeedMessagesMapping()
    {
        Table("tbl_FeedMessages");
            Id(x => x.Id);
        Map(x => x.FeedType);

        Version(x => x.Version);
    }
}
}
