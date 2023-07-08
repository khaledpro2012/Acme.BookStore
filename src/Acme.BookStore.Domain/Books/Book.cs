using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Acme.BookStore.Books
{
    public class Book : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [NotNull]
        public virtual string BookName { get; set; }

        public Book()
        {

        }

        public Book(Guid id, string bookName)
        {

            Id = id;
            Check.NotNull(bookName, nameof(bookName));
            BookName = bookName;
        }

    }
}