using System;
using FluentAssertions;
using Xunit;
using DDDCourse.Logic.Atm;
using DDDCourse.Logic.Utils;
using static DDDCourse.Logic.Shared.Money;

namespace DDDSpecs.Tests
{
    public class AtmSpecs
    {
        [Fact]
        public void Take_money_exchanges_money_with_commission()
        {
            Initer.init("");
            var atm = new Atm();
            atm.LoadMoney(Dollar);

            atm.TakeMoney(1m);

            atm.MoneyInside.Amount.Should().Be(0m);
            atm.MoneyCharged.Should().Be(1.01m);
        }

        [Fact]
        public void Commission_is_at_least_one_cent()
        {
            Initer.init("");
            var atm = new Atm();
            atm.LoadMoney(Cent);

            atm.TakeMoney(0.01m);

            atm.MoneyCharged.Should().Be(0.02m);
        }

        [Fact]
        public void Commission_is_rounded_up_to_the_next_cent()
        {
            Initer.init("");
            var atm = new Atm();
            atm.LoadMoney(Dollar + TenCent);

            atm.TakeMoney(1.1m);

            atm.MoneyCharged.Should().Be(1.12m);
        }

        // Test only works if events would be dispatched correctly after DB update and no immediately after adding it
        // [Fact]
        // public void Take_money_raises_an_event()
        // {
            // Initer.init("");
            // Atm atm = new Atm();
            // atm.LoadMoney(Dollar);

            // ??
            // DomainEvents.Register<BalanceChangedEvent>(e => balanceChangedEvent = e);
            // atm.TakeMoney(1m);

            // var balanceChangedEvent = atm.DomainEvents[0] as BalanceChangedEvent;
            // balanceChangedEvent.Should().NotBeNull();
            // balanceChangedEvent.Delta.Should().Be(1.01m);
        // }
    }
}
