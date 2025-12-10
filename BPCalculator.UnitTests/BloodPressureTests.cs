using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPCalculator.UnitTests
{
    public class BloodPressureTests
    {
        // =================================================================
        // Test cases for BPCategory calculation (The main logic)
        // =================================================================

        [Theory]
        [InlineData(140, 90)]  // High: Systolic >= 140 AND Diastolic >= 90
        [InlineData(160, 80)]  // High: Systolic >= 140 only
        [InlineData(130, 95)]  // High: Diastolic >= 90 only
        [InlineData(190, 100)] // High: Max valid values
        public void Category_ShouldReturnHigh_WhenBPisHigh(int systolic, int diastolic)
        {
            // Arrange
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };

            // Act
            BPCategory category = bp.Category;

            // Assert
            Assert.Equal(BPCategory.High, category);
        }

        [Theory]
        [InlineData(130, 85)]  // PreHigh: S: 130-139 AND D: 85-89 (lower boundary)
        [InlineData(139, 89)]  // PreHigh: S: 130-139 AND D: 85-89 (upper boundary)
        [InlineData(135, 80)]  // PreHigh: Systolic 130-139 only
        [InlineData(120, 88)]  // PreHigh: Diastolic 85-89 only
        public void Category_ShouldReturnPreHigh_WhenBPisPreHigh(int systolic, int diastolic)
        {
            // Arrange
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };

            // Act
            BPCategory category = bp.Category;

            // Assert
            Assert.Equal(BPCategory.PreHigh, category);
        }

        [Theory]
        [InlineData(90, 60)]   // Ideal: S: 90-129 AND D: 60-84 (lower boundary)
        [InlineData(129, 84)]  // Ideal: S: 90-129 AND D: 60-84 (upper boundary)
        [InlineData(110, 70)]  // Ideal: Typical reading
        [InlineData(120, 80)]  // Ideal: Another typical reading
        public void Category_ShouldReturnIdeal_WhenBPisIdeal(int systolic, int diastolic)
        {
            // Arrange
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };

            // Act
            BPCategory category = bp.Category;

            // Assert
            Assert.Equal(BPCategory.Ideal, category);
        }

        [Theory]
        [InlineData(89, 59)]   // Low: S < 90 AND D < 60 (upper boundary of low range)
        [InlineData(70, 40)]   // Low: Min valid values
        [InlineData(80, 50)]   // Low: Typical low reading
        [InlineData(100, 55)]  // Low: Diastolic < 60 only (Systolic in Ideal range)
        [InlineData(85, 75)]   // Low: Systolic < 90 only (Diastolic in Ideal range)
        public void Category_ShouldReturnLow_WhenBPisLow(int systolic, int diastolic)
        {
            // Arrange
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };

            // Act
            BPCategory category = bp.Category;

            // Assert
            Assert.Equal(BPCategory.Low, category);
        }

        // =================================================================
        // Test case for the invalid reading check (Systolic <= Diastolic)
        // =================================================================

        [Theory]
        [InlineData(80, 80)]   // Systolic == Diastolic
        [InlineData(70, 75)]   // Systolic < Diastolic
        [InlineData(110, 120)] // Systolic < Diastolic (in Ideal range)
        public void Category_ShouldReturnLow_WhenSystolicIsNotGreaterThanDiastolic(int systolic, int diastolic)
        {
            // Arrange
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };

            // Act
            BPCategory category = bp.Category;

            // Assert
            // The implementation specifies BPCategory.Low as the default fallback for this invalid case.
            Assert.Equal(BPCategory.Low, category);
        }
    }
}
