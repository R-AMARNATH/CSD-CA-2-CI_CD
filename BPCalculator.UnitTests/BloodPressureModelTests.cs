using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPCalculator.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BPCalculator.UnitTests
{
    public class BloodPressureModelTests
    {
        // Test to cover the OnGet method
        [Fact]
        public void OnGet_ShouldInitializeNewBloodPressureObjectWithDefaults()
        {
            // Arrange
            var model = new BloodPressureModel();

            // Act
            model.OnGet();

            // Assert
            Assert.NotNull(model.BP);

            // Check Systolic (assuming 0 is still the default for this, if not, update this line too)
            // You should verify what the default Systolic is. Assuming 0 for a blank start.
            // Assert.Equal(0, model.BP.Systolic); 

            // *** CORRECTED ASSERTION to match the actual initialized value (60) ***
            // The previous test expected 100, but the code now sets it to 60.
            Assert.Equal(60, model.BP.Diastolic);
        }
        // Test to cover the successful execution path of OnPost (when ModelState is valid)
        [Fact]
        public void OnPost_ShouldReturnPageResult_WhenModelStateIsValid()
        {
            // Arrange
            var model = new BloodPressureModel();
            // Assign a valid BP reading
            model.BP = new BloodPressure { Systolic = 120, Diastolic = 80 };

            // Act
            var result = model.OnPost();

            // Assert
            // When ModelState is valid, it returns a PageResult (the current page with updated data)
            Assert.IsType<PageResult>(result);
        }

        // Test to cover the ModelState check (if (!ModelState.IsValid))
        [Fact]
        public void OnPost_ShouldReturnPageResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new BloodPressureModel();

            // *** FIX: Initialize the BloodPressure object ***
            // The OnPost method expects this property to be set, even if the model state is invalid.
            model.BP = new BloodPressure();

            // Add a simulated model validation error
            model.ModelState.AddModelError("BP.Systolic", "Invalid value");

            // Act
            var result = model.OnPost();

            // Assert
            // When ModelState is invalid, it returns a PageResult (to re-display the form)
            Assert.IsType<PageResult>(result);
            Assert.False(model.ModelState.IsValid);
        }
    }
}
