using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
        [Display(Name="Low Blood Pressure")] Low,
        [Display(Name="Ideal Blood Pressure")]  Ideal,
        [Display(Name="Pre-High Blood Pressure")] PreHigh,
        [Display(Name ="High Blood Pressure")]  High
    };

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }                       // mmHG

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; }                      // mmHG

        // calculate BP category
        public BPCategory Category
        {
            get
            {
                // First, validate that systolic is greater than diastolic
                if (Systolic <= Diastolic)
                {
                    // This is an invalid reading, but we need to return something
                    // Based on standard medical guidelines, this is invalid
                    // For this implementation, we'll treat it as an error case
                    // In a real application, you might want to throw an exception
                    Debug.WriteLine($"Invalid reading: Systolic ({Systolic}) must be greater than Diastolic ({Diastolic})");
                    return BPCategory.Low; // Default fallback
                }

                // Check for High Blood Pressure (Stage 2)
                // Systolic >= 140 OR Diastolic >= 90
                if (Systolic >= 140 || Diastolic >= 90)
                {
                    return BPCategory.High;
                }

                // Check for Pre-High Blood Pressure (Stage 1)
                // Systolic 130-139 OR Diastolic 85-89
                // Note: Different guidelines use slightly different ranges
                // Using the more common: Systolic 130-139 OR Diastolic 85-89
                if ((Systolic >= 130 && Systolic <= 139) ||
                    (Diastolic >= 85 && Diastolic <= 89))
                {
                    return BPCategory.PreHigh;
                }

                // Check for Ideal Blood Pressure
                // Systolic 90-129 AND Diastolic 60-84
                if ((Systolic >= 90 && Systolic <= 129) &&
                    (Diastolic >= 60 && Diastolic <= 84))
                {
                    return BPCategory.Ideal;
                }

                // Everything else is considered Low Blood Pressure
                // This includes:
                // - Systolic < 90 OR Diastolic < 60
                // - Any other combination not covered above
                return BPCategory.Low;                      // replace this
            }
        }
    }
}
