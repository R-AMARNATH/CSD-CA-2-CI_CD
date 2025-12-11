using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;
using BPCalculator;

namespace BPCalculator.ReqnrollTests
{
    [Binding]
    public class BloodPressureSteps
    {
        private BloodPressure _bp;
        private BPCategory _category;
        private IList<ValidationResult> _validationResults;
        private bool _isValid;

        public BloodPressureSteps()
        {
            _bp = new BloodPressure();
            _validationResults = new List<ValidationResult>();
        }

        [Given(@"a systolic value of (.*)")]
        public void GivenASystolicValueOf(int systolic)
        {
            _bp.Systolic = systolic;
        }

        [Given(@"a diastolic value of (.*)")]
        public void GivenADiastolicValueOf(int diastolic)
        {
            _bp.Diastolic = diastolic;
        }

        [When(@"I check the category")]
        public void WhenICheckTheCategory()
        {
            _category = _bp.Category;
        }

        [Then(@"the category should be ""(.*)""")]
        public void ThenTheCategoryShouldBe(string expected)
        {
            string actualDisplay = GetDisplayName(_category);
            Assert.AreEqual(expected, actualDisplay);
        }

        [When(@"I validate the reading")]
        public void WhenIValidateTheReading()
        {
            var ctx = new ValidationContext(_bp);
            _validationResults = new List<ValidationResult>();
            _isValid = Validator.TryValidateObject(_bp, ctx, _validationResults, true);
        }

        [Then(@"validation should fail")]
        public void ThenValidationShouldFail()
        {
            Assert.IsFalse(_isValid);
        }

        [Then(@"it should contain ""(.*)""")]
        public void ThenItShouldContain(string message)
        {
            string messages = string.Join(" | ", _validationResults);
            StringAssert.Contains(messages, message);
        }
        
        [Then(@"the advice message should be ""(.*)""")]
        public void ThenTheAdviceMessageShouldBe(string expected)
        {
            // Advice is derived from the BP reading
            Assert.AreEqual(expected, _bp.AdviceMessage);
        }

        private string GetDisplayName(BPCategory category)
        {
            var mem = typeof(BPCategory).GetMember(category.ToString());
            if (mem.Length > 0)
            {
                var attr = Attribute.GetCustomAttribute(mem[0], typeof(DisplayAttribute)) as DisplayAttribute;
                if (attr != null && !string.IsNullOrEmpty(attr.Name))
                {
                    return attr.Name;
                }
            }

            return category.ToString();
        }

    }
}
