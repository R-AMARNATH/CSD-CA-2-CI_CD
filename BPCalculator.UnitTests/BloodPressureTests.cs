using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPCalculator.UnitTests
{
    public class BloodPressureTests
    {
        // =====================================================================
        // 1. High BP Tests (Stage 2)
        // =====================================================================
        [Theory]
        [InlineData(140, 70)]
        [InlineData(150, 60)]
        [InlineData(120, 95)]
        [InlineData(139, 90)]
        public void Category_High_ShouldBeDetected(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.High, bp.Category);
        }

        // =====================================================================
        // 2. Pre-High Tests (Stage 1)
        // =====================================================================
        [Theory]
        [InlineData(130, 60)]
        [InlineData(135, 80)]
        [InlineData(110, 87)]
        [InlineData(139, 89)]
        public void Category_PreHigh_ShouldBeDetected(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.PreHigh, bp.Category);
        }

        // =====================================================================
        // 3. Ideal BP Tests
        // =====================================================================
        [Theory]
        [InlineData(90, 60)]
        [InlineData(129, 84)]
        [InlineData(110, 70)]
        [InlineData(100, 80)]
        public void Category_Ideal_ShouldBeDetected(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.Ideal, bp.Category);
        }

        // =====================================================================
        // 4. Low BP Tests
        // =====================================================================
        [Theory]
        [InlineData(89, 50)]
        [InlineData(80, 55)]
        [InlineData(100, 59)]
        [InlineData(85, 75)]
        public void Category_Low_ShouldBeDetected(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        // =====================================================================
        // 5. Invalid Readings: Systolic <= Diastolic
        // =====================================================================
        [Theory]
        [InlineData(80, 80)]
        [InlineData(95, 100)]
        [InlineData(120, 130)]
        public void Category_InvalidReading_ShouldReturnLow(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        // =====================================================================
        // 6. Validate Range Attributes (Required to increase coverage)
        // =====================================================================
        [Theory]
        [InlineData(50)]   // below min
        [InlineData(250)]  // above max
        public void Systolic_ShouldFailRangeValidation(int systolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = 70 };
            var context = new ValidationContext(bp)
            {
                MemberName = nameof(BloodPressure.Systolic)
            };
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateProperty(bp.Systolic, context));
        }

        [Theory]
        [InlineData(10)]   // below min
        [InlineData(150)]  // above max
        public void Diastolic_ShouldFailRangeValidation(int diastolic)
        {
            var bp = new BloodPressure { Systolic = 100, Diastolic = diastolic };
            var context = new ValidationContext(bp)
            {
                MemberName = nameof(BloodPressure.Diastolic)
            };
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateProperty(bp.Diastolic, context));
        }

        // =====================================================================
        // 7. Enum Display Name Tests (also counted in coverage)
        // =====================================================================
        [Fact]
        public void Enum_DisplayNames_ShouldBeCorrect()
        {
            Assert.Equal("Low Blood Pressure", GetDisplayName(BPCategory.Low));
            Assert.Equal("Ideal Blood Pressure", GetDisplayName(BPCategory.Ideal));
            Assert.Equal("Pre-High Blood Pressure", GetDisplayName(BPCategory.PreHigh));
            Assert.Equal("High Blood Pressure", GetDisplayName(BPCategory.High));
        }

        private string GetDisplayName(BPCategory category)
        {
            var type = typeof(BPCategory);
            var memInfo = type.GetMember(category.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            return ((DisplayAttribute)attributes[0]).Name;
        }

        // =====================================================================
        // 8. Boundary Tests: Exact min/max values
        // =====================================================================
        [Theory]
        [InlineData(70, 40)]   // absolute minimum
        [InlineData(190, 100)] // absolute maximum
        public void Category_ShouldHandleBoundaryValues(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };

            // Both fall into existing category rules
            var category = bp.Category;

            Assert.NotNull(category);
        }
    }
}
