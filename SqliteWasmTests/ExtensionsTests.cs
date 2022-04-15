using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqliteWasmHelper;
using SqliteWasmTests.TestHelpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace SqliteWasmTests
{
    public class ExtensionsTests
    {
        public static IEnumerable<object[]> Registrations()
        {
            const string datasource = "Data Source=test.db";
            const string filename = "Filename=test.db";

            foreach (var dataSourceOption in new[] { string.Empty, datasource, filename })
            {
                if (dataSourceOption == string.Empty)
                {
                    yield return new object[]
                    {
                        (IServiceCollection coll) => coll.AddSqliteWasmDbContextFactory<TestContext>(opts =>
                        opts.UseSqlite()),
                        string.Empty,
                        ServiceLifetime.Singleton,
                    };
                }
                else
                {
                    yield return new object[]
                    {
                        (IServiceCollection coll) => coll.AddSqliteWasmDbContextFactory<TestContext>(opts =>
                        opts.UseSqlite(dataSourceOption)),
                        dataSourceOption,
                        ServiceLifetime.Singleton,
                    };
                }
                foreach (var lifetimeOption in new[]
                {
                    ServiceLifetime.Singleton,
                    ServiceLifetime.Scoped,
                    ServiceLifetime.Transient
                })
                {
                    if (dataSourceOption == string.Empty)
                    {
                        yield return new object[]
                        {
                            (IServiceCollection coll) => coll.AddSqliteWasmDbContextFactory<TestContext>(
                                opts => opts.UseSqlite(),
                                lifetime: lifetimeOption),
                            string.Empty,
                            lifetimeOption,
                        };

                        yield return new object[]
                        {
                            (IServiceCollection coll) => coll.AddSqliteWasmDbContextFactory<TestContext>(
                                (_, opts) => opts.UseSqlite(),
                                lifetime: lifetimeOption),
                            string.Empty,
                            lifetimeOption,
                        };
                    }
                    else
                    {
                        yield return new object[]
                        {
                            (IServiceCollection coll) => coll.AddSqliteWasmDbContextFactory<TestContext>(opts =>
                            opts.UseSqlite(dataSourceOption), lifetimeOption),
                            dataSourceOption,
                            lifetimeOption,
                        };

                        yield return new object[]
                        {
                            (IServiceCollection coll) => coll.AddSqliteWasmDbContextFactory<TestContext>(
                                (_, opts) =>
                            opts.UseSqlite(dataSourceOption), lifetimeOption),
                            dataSourceOption,
                            lifetimeOption,
                        };
                    }

                }
            }
        }

        public static IEnumerable<object[]> RegistrationsLite()
        {
            foreach (var registration in Registrations())
            {
                yield return new object[]
                {
                    registration[0]
                };
            }
        }

        [Theory]
        [MemberData(nameof(RegistrationsLite))]
        public void ExtensionsRegisterBrowserCache(
            Func<IServiceCollection, IServiceCollection> registration
            )
        {
            // arrange
            var coll = new TestServiceCollection();

            // act
            registration(coll);

            // assert
            Assert.Contains(coll,
                c => c.ServiceType == typeof(IBrowserCache)
                && c.ImplementationType == typeof(BrowserCache)
                && c.Lifetime == ServiceLifetime.Singleton);
        }


        [Theory]
        [MemberData(nameof(RegistrationsLite))]
        public void ExtensionsRegisterSqliteSwap(
            Func<IServiceCollection, IServiceCollection> registration
            )
        {
            // arrange
            var coll = new TestServiceCollection();

            // act
            registration(coll);

            // assert
            Assert.Contains(coll,
                c => c.ServiceType == typeof(ISqliteSwap)
                && c.ImplementationType == typeof(SqliteSwap)
                && c.Lifetime == ServiceLifetime.Singleton);
        }


        [Theory]
        [MemberData(nameof(RegistrationsLite))]
        public void ExtensionsRegisterSqlWasmDbContextFactory(
            Func<IServiceCollection, IServiceCollection> registration
            )
        {
            // arrange
            var coll = new TestServiceCollection();

            // act
            registration(coll);

            // assert
            Assert.Contains(coll,
                c => c.ServiceType == typeof(ISqliteWasmDbContextFactory<TestContext>)
                && c.ImplementationType == typeof(SqliteWasmDbContextFactory<TestContext>)
                && c.Lifetime == ServiceLifetime.Singleton);
        }

        [Theory]
        [MemberData(nameof(Registrations))]

        public void ExtensionsCallTheUnderlyingDbContextFactory(
            Func<IServiceCollection, IServiceCollection> registration,
            string expectedDataSource,
            ServiceLifetime expectedLifetime
            )
        {
            // arrange
            var coll = new TestServiceCollection();

            // act
            registration(coll);

            // assert
            Assert.Contains(coll,
                c => c.ServiceType == typeof(IDbContextFactory<TestContext>)
                && c.Lifetime == expectedLifetime);

            if (!string.IsNullOrEmpty(expectedDataSource))
            {
                var sp = coll.BuildServiceProvider().CreateScope().ServiceProvider;
                var factory = sp.GetRequiredService<IDbContextFactory<TestContext>>();
                using var ctx = factory.CreateDbContext();
                Assert.Equal(expectedDataSource, ctx.Database.GetConnectionString());
            }
        }
    }
}