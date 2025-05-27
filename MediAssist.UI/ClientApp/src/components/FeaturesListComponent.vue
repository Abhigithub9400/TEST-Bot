<template>
    <div class="features-container">
      <div 
        v-for="(feature, index) in features" 
        :key="index" 
        class="feature-row"
        v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }"
      >
        <!-- Left image, right content layout for even indexes -->
        <template v-if="index % 2 === 0">
          <div class="image-container">
            <div class="image-wrapper">
              <img :src="feature.image" :alt="feature.title" class="feature-image" />
            </div>
          </div>
  
          <div class="content-container">
            <div class="content-inner">
              <h2 class="feature-title h2-sm-medium">{{ feature.title }}</h2>
              
              <div class="feature-benefits">
                <div 
                  v-for="(benefit, benefitIndex) in feature.benefits" 
                  :key="benefitIndex" 
                  class="benefit-item"
                >
                  <div class="benefit-dot"></div>
                  <h7 class="benefit-text h7-regular">{{ benefit }}</h7>
                </div>
              </div>
            </div>
          </div>
        </template>
  
        <!-- Left content, right image layout for odd indexes -->
        <template v-else>
          <div class="content-container">
            <div class="content-inner">
              <h2 class="feature-title h2-sm-medium">{{ feature.title }}</h2>
              
              <div class="feature-benefits">
                <div 
                  v-for="(benefit, benefitIndex) in feature.benefits" 
                  :key="benefitIndex" 
                  class="benefit-item"
                >
                  <div class="benefit-dot"></div>
                  <h7 class="benefit-text h7-regular">{{ benefit }}</h7>
                </div>
              </div>
            </div>
          </div>
  
          <div class="image-container">
            <div class="image-wrapper">
              <img :src="feature.image" :alt="feature.title" class="feature-image" />
            </div>
          </div>
        </template>
      </div>
    </div>
  </template>
  
  <script>
  import feature1 from "@/assets/FeaturesPageImages/feature-1.jpg";
  import feature2 from "@/assets/FeaturesPageImages/feature-2.jpg";
  import feature3 from "@/assets/FeaturesPageImages/feature-3.jpg";
  import feature4 from "@/assets/FeaturesPageImages/feature-4.jpg";
  import feature5 from "@/assets/FeaturesPageImages/feature-5.jpg";
  import feature6 from "@/assets/FeaturesPageImages/feature-6.jpg";
  
  export default {
    name: "FeatureList",
    data() {
      return {
        features: [
          {
            title: "Real-Time Transcription & Prescription Generation",
            description: "",
            benefits: [
              "Instantly converts spoken words into structured medical notes, minimizing manual entry.",
              "Analyzes doctor-patient conversations to auto-generate accurate prescriptions."
            ],
            image: feature1
          },
          {
            title: "AI-Driven Clinical Support",
            description: "",
            benefits: [
              "Offers potential diagnosis suggestions during consultations, with an optional toggle feature for doctors.",
              "Automatically creates structured SOAP notes, ensuring compliance with medical documentation standards."
            ],
            image: feature2
          },
          {
            title: "Customizable & Multilingual Templates",
            description: "",
            benefits: [
              "Allows tailoring of medical templates to fit specific practice workflows and specialties.",
              "Supports multiple languages, enhancing accessibility for diverse patient populations."
            ],
            image: feature3
          },
          {
            title: "AI-Secure, Compliant, & Integrated Data Handling",
            description: "",
            benefits: [
              "Ensures HIPAA-compliant data privacy and security.",
              "Seamlessly integrates with popular Electronic Health Record (EHR) systems for direct transfer of notes into patient records."
            ],
            image: feature4
          },
          {
            title: "Multi-User, Multi-Device Accessibility",
            description: "",
            benefits: [
              "Supports multiple accounts for healthcare teams, promoting consistent documentation across departments.",
              "Accessible on desktops, laptops, tablets, and smartphones, with cloud-based access from anywhere."
            ],
            image: feature5
          },
          {
            title: "Adaptive AI for Continuous Improvement",
            description: "",
            benefits: [
              "Learns from user corrections to enhance transcription accuracy over time.",
              "Adapts to various accents, speech patterns, and medical terminologies, ensuring precise documentation."
            ],
            image: feature6
          }
        ]
      };
    }
  };
  </script>
  
  <style scoped>
  .features-container {
    display: flex;
    flex-direction: column;
    gap: 80px;
    width: 100%;
  }
  
  .feature-row {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
  }
  
  .image-container {
    display: flex;
    align-items: center;
    width: 504px;
    height: 393px;
  }
  
  .image-wrapper {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 100%;
    background-color: #eaf4ff;
    border: 1px solid #e4e6fe;
    border-radius: 16px;
  }
  
  .feature-image {
    width: 500px;
    height: 390px;
    object-fit: cover;
  }
  
  .content-container {
    width: 609px;
    display: flex;
    flex-direction: column;
  }
  
  .content-inner {
    display: flex;
    flex-direction: column;
    gap: 10px;
    width: 100%;
  }
  
  .feature-title {
    width: 100%;
    margin-bottom: 8px;
  }
  
  .feature-description {
    font-size: 18px;
    line-height: 28px;
    width: 100%;
    margin: 0px;
  }
  
  .feature-benefits {
    display: flex;
    flex-direction: column;
    gap: 8px;
    width: 100%;
  }
  
  .benefit-item {
    display: flex;
    align-items: flex-start;
    gap: 8px;
    width: 100%;
  }

.benefit-dot {
  width: 5px;
  height: 5px;
  background-color: #000;
  border-radius: 50%;
  flex-shrink: 0;
  margin-top: 12px;
}

.benefit-text {
  color: #555;
  flex: 1;
  margin: 0;
}

  
  @media (max-width: 1200px) {
    .feature-row {
      flex-direction: column;
      gap: 40px;
    }
    
    .image-container, .content-container {
      width: 100%;
    }
    
    /* Force the order for mobile: always content first, then image */
    .feature-row:nth-child(odd) {
      flex-direction: column-reverse;
    }
  }
  </style>