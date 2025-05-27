<template>
    <div class="website fee" v-scroll-reveal>
      <div class="fee__content">
        <section class="fee-header" id="fee-header" ref="fee-header">
          <div class="fee-header-content">
            <h1 class="fee-text h1-semibold">Flexible Pricing Plans Tailored for Healthcare Professionals</h1>
            <p class="fee-text-2 lg-regular">Transparent and cost-effective pricing plans designed for individual practitioners and healthcare organizations.</p>
            <button class="fee-demo-button btn btn-primary" @click="handleRequestDemo('request-demo')">
              <div class="btn-background">
                <div class="inside-text">
                  <span>Book a Demo</span>
                </div>
              </div>
              <div class="border-gradient-fn"></div>
            </button>
          </div>
        </section>
        <section class="fee-content" id="feeContent" ref="feeContent">
          <div class="pricing">
            <div class="fee__billing">
                <div class="fee__header-title">
                  <h4 class="pricing-title h4-medium">Choose the plan that works for you.</h4>
                </div>
                <div class="fee__switch_actions">
                  <div class="fee__switch">
                     <span 
                        class="fee__billing-option lg-medium" 
                        :class="{ 'fee__billing-option--active': !isAnnual }"
                        @click="isAnnual = false"
                      >
                        Monthly
                     </span>
                    <input 
                      type="checkbox" 
                      v-model="isAnnual"
                      @change="updatePrices"
                      class="fee__switch-input"
                    >
                    <span class="fee__switch-slider"></span>
                    
                    <span 
                      class="fee__billing-option lg-medium" 
                      :class="{ 'fee__billing-option--active': isAnnual }"
                      @click="isAnnual = true"
                    >
                      Annually
                    </span>
                  </div>
                </div>
            </div>
            
            <div class="fee__plans">
              <div class="fee__plans-grid">
                <!-- Free Plan -->
                <div class="plan plan--free" :class="{ 'plan--current': currentPlan === 'free' }">
                  <div class="plan__details">
                    <h7 class="plan__title h7-bold">Free Plan</h7>
                    <div class="plan__price">
                      <h1 class="plan__currency h1-semibold">$</h1>
                      <h1 class="plan__amount h1-semibold">0</h1>
                      <span class="plan__period md-regular">/ Month</span>
                    </div>
                  </div>
                  <div class="plan__features">
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="tooltip sm-medium">
                        <span class="feature-value sm-medium">*</span>Up to<span class="feature-value">&nbsp;5</span> Transcriptions & Report Generation/month
                        <span class="tooltiptext">Pricing varies with token usage and covers up to 5 million tokens.</span>
                      </div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Available Access Hours<span class="feature-value">&nbsp;30</span> minutes</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Up to<span class="feature-value">&nbsp;10 </span>minutes session limit</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Real-time results</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">AI-powered draft SOAP note generation</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Automatic data deletion for security compliance</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Seamless EHR system integration</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Generate documents with confidence</div>
                    </div>
                     <div class="plan__feature plan__feature--excluded">
                      <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Email support</div>
                    </div>
                     <div class="plan__feature plan__feature--excluded">
                      <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Priority access to latest models</div>
                    </div>
                    <div class="plan__feature plan__feature--excluded">
                      <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Early access to new AI features</div>
                    </div>
                    <div class="plan__feature plan__feature--excluded">
                      <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Share Reports via Email</div>
                    </div>
                    <div class="plan__feature plan__feature--excluded">
                      <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Watermark removal</div>
                    </div>
                     <div class="plan__feature plan__feature--excluded">
                      <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Tailored Capabilities & advanced support</div>
                    </div>
                  </div>
                  <div class="plan-button">
                    <div v-if="currentPlan !== 'free'" class="plan__cta-container">
                        <button class="plan__cta sm-semibold" @click="redirectToSignUp">Get Started Now</button>
                    </div>
                    <div v-else class="plan__current-label sm-semibold">
                        Current Plan
                    </div>
                  </div>
                </div>
      
                <!-- Pro Plan -->
                <div class="plan plan--pro" :class="{ 'plan--current': currentPlan === 'pro' }">
                  <div class="plan__details">
                    <h7 class="plan__title h7-bold">Pro Plan</h7>
                    <div class="plan__price">
                      <h1 class="plan__currency h1-semibold">$</h1>
                      <h1 class="plan__amount h1-semibold">{{ proPrice }}</h1>
                      <span class="plan__period md-regular">/ Month</span>
                    </div>
                  </div>
                  <div class="plan__features">
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="tooltip sm-medium">
                        <span class="feature-value tooltip-trigger sm-medium">*</span>Up to<span class="feature-value">&nbsp;40</span> transcriptions per month
                        <span class="tooltiptext">Pricing varies with token usage and covers up to 5 million tokens.</span>
                      </div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium"><span class="feature-value">10</span> hours available</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Up to<span class="feature-value">&nbsp;15</span> minutes session limit</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Real-time results</div>
                    </div>
                     <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">AI-powered draft SOAP note generation</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Automatic data deletion for security compliance</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Seamless EHR system integration</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Generate documents with confidence</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Email support</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Priority access to latest models</div>
                    </div>
                    <div class="plan__feature plan__feature--excluded">
                      <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Early access to new AI features</div>
                    </div>
                     <div class="plan__feature plan__feature--excluded">
                     <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Share Reports via Email</div>
                    </div>
                    <div class="plan__feature plan__feature--excluded">
                     <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Watermark removal</div>
                    </div>
                    <div class="plan__feature plan__feature--excluded">
                     <img src="@/assets/Pricing/exclude.svg" alt="checkmark">
                      <div class="plan-include plan-exclude sm-medium">Tailored Capabilities & advanced support</div>
                    </div>
                  </div>
                   <div class="plan-button">
                     <div v-if="currentPlan !== 'pro'" class="plan__cta-container">
                      <button class="plan__cta sm-semibold" @click="ContactUs('contact-us-subscription','Pro')">Contact us</button>
                     </div>
                     <div v-else class="plan__current-label sm-semibold">
                      Current Plan
                     </div>
                   </div>
                </div>
      
                <!-- Advanced Plan -->
                <div class="plan plan--premium" :class="{ 'plan--current': currentPlan === 'advanced' }">
                  <div class="plan__details">
                    <h7 class="plan__title h7-bold">Advanced Plan</h7>
                    <div class="plan__price">
                      <h1 class="plan__currency h1-semibold">$</h1>
                      <h1 class="plan__amount h1-semibold">{{ premiumPrice }}</h1>
                      <span class="plan__period md-regular">/ Month</span>
                    </div>
                  </div>
                  <div class="plan__features">
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="tooltip sm-medium">
                        <span class="feature-value sm-medium">*</span>Up to<span class="feature-value">&nbsp;80</span> transcriptions per month
                        <span class="tooltiptext">Pricing varies with token usage and covers up to 5 million tokens.</span>
                      </div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium"><span class="feature-value">20</span> hours available</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Up to<span class="feature-value">&nbsp;15</span> minutes session limit</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Real-time results</div>
                    </div>
                     <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">AI-powered draft SOAP note generation</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Automatic data deletion for security compliance</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Seamless EHR system integration</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                       <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Generate documents with confidence</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                       <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Email with priority support</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                       <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Priority access to latest models</div>
                    </div>
                     <div class="plan__feature plan__feature--excluded">
                       <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Early access to new AI features</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Share Reports via Email</div>
                    </div>
                     <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/highlightedinclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Watermark removal</div>
                    </div>
                     <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/highlightedexclude.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Tailored Capabilities & advanced support</div>
                    </div>
                  </div>
                  <div class="plan-button">
                     <div v-if="currentPlan !== 'premium'" class="plan__cta-container">
                      <button class="plan__cta sm-semibold" @click="ContactUs('contact-us-subscription','Advanced')">Contact us</button>
                    </div>
                    <div v-else class="plan__current-label sm-semibold">
                      Current Plan
                    </div>
                  </div>
                </div>
      
                <!-- Enterprise Plan -->
                <div class="plan" :class="{ 'plan--current': currentPlan === 'enterprise' }">
                  <div class="plan__details">
                    <h7 class="plan__title h7-bold">Enterprise Plan</h7>
                    <div class="plan__price">
                      <h4 class="enterprice-plan__price h4-bold">{{ enterprisePrice }}</h4>
                    </div>
                  </div>
                  <div class="plan__features">
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Fully customizable based on needs</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Flexible to your requirements</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Adjustable as per your preference</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Real-time results</div>
                    </div>
                     <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">AI-powered draft SOAP note generation</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Automatic data deletion for security compliance</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Seamless EHR system integration</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Generate documents with confidence</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">24x7 support</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                     <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Priority access to latest models</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Early access to new AI features</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Share Reports via Email</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Watermark removal</div>
                    </div>
                    <div class="plan__feature plan__feature--included">
                      <img src="@/assets/Pricing/include.svg" alt="checkmark">
                      <div class="plan-include sm-medium">Tailored Capabilities & advanced support</div>
                    </div>
                  </div>
                  <div class="plan-button">
                    <div v-if="currentPlan !== 'premium'" class="plan__cta-container">
                      <button class="plan__cta sm-semibold" @click="ContactUs('contact-us-subscription','Enterprise')">Contact us</button>
                    </div>
                    <div v-else class="plan__current-label sm-semibold">
                      Current Plan
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>
      </div>
    </div>
</template>
  
<script setup>
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';
const feeContent = ref(null);

const router = useRouter();
  
const isAnnual = ref(false);
  
const currentPlan = ref('');
  
const proPrice = computed(() => {
  return isAnnual.value ? 22 : 24;
});
  
const ContactUs = (modalType, selectedPlan) => {
  localStorage.setItem('modalType', modalType);
  localStorage.setItem('selectedPlan', selectedPlan);
  router.push('/contact-us');
};

const premiumPrice = computed(() => {
  return isAnnual.value ? 43 : 48;
});
  
const enterprisePrice = computed(() => {
  return 'Talk to Sales';
});
  
const redirectToSignUp = () => {
  router.push('/signup');
};
  
const updatePrices = () => {
    // Price update logic if needed
  console.log("Billing option changed:", this.isAnnual ? "Annually" : "Monthly");
};

const handleRequestDemo = (modalType) => {
  localStorage.setItem('modalType', modalType);
  router.push('/request-demo');
};
  
</script>
  
<style scoped>
.fee {
  background-color: #f5f7fb;
}
  
.fee__sidebar {
  width: 80px;
  position: fixed;
  height: 100vh;
  background-color: #1a1f36;
}
  
.fee__content {
  flex: 1;
}
  
.fee-header {
  width: 100%;
  background: linear-gradient(0deg, #050613 0%, #051B2F 100%);
  height: 400px;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  flex-direction: column;
}
  
.fee-header-content {
  top: 40px;
  position: relative;
  margin-inline: auto;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  min-width: 1280px;
}

.fee-text {
  color: #ffffff;
  position: relative;
  width: 847px;
  margin: 0px;
}
 
.fee-text-2{
  color: #ffffff;
  position: relative;
  width: 945px;
  margin-bottom: 30px;
}

.fee-demo-button {
  width: 130px;
}
 
.fee-content{
  align-items: flex-start;
  background-color: #f7fbff;
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 70px 100px;
  position: relative;
}

.pricing{
  align-items: center;
  border-radius: 3.44px;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  position: relative;
  margin-inline: auto;
  max-width: 1280px;
}

.fee__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: white;
  padding: 20px 30px;
  border-bottom: 1px solid #e5e7eb;
  position: sticky;
  top: 0;
  z-index: 1000;
}
  
.fee__header-title {
  align-items: flex-start;
  display: flex;
  align-self: stretch;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  justify-content: center;
  position: relative;
}
  
.pricing-title {
  color: var(--Text-Primary, #1C1C1C);
  letter-spacing: 0;
  position: relative;
  white-space: nowrap;
  width: fit-content;
}
  
.fee__billing {
  align-items: center;
  display: flex;
  gap: 670px;
  justify-content: center;
  position: relative;
}
  
.fee__billing-option {
  flex: 1;
  text-align: center;
  cursor: pointer;
  position: relative;
  z-index: 2;
  color: black;
}
  
.fee__billing-option--active {
  color: white;
}
  
.feature-value {
  color: #1c1c1c;
  letter-spacing: 0;
  position: relative;
}

.fee__switch {
  position: relative;
  display: flex;
  background: var(--Surface-Background-Grey, #EFEFEF);
  border-radius: 94.41px;
  width: 200px;
  height: 40px;
  align-items: center;
  justify-content: space-between;
  padding: 4px;
}
  
.fee__switch-input {
  opacity: 0;
  width: 0;
  height: 0;
  position: absolute;
}
  
.fee__switch-slider {
  position: absolute;
  top: 4px;
  left: 5px;
  width: 97px;
  height: 41px;
  background: #0066D4;
  border-radius: 94.41px;
  transition: transform 0.3s ease;
  z-index: 1;
}
  
.fee__switch-input:checked + .fee__switch-slider {
  transform: translateX(100px);  /* Updated to match new color scheme */
}
  
.fee__plans {
  margin: 40px auto;
  width: 100%;
  margin-bottom: 0px;
}
  
.fee__plans-grid {
  display: grid;
  grid-template-columns: repeat(4, minmax(300px, 405px));
  gap: 25px;
  justify-content: center;
}
  
.plan {
  height: auto;
  padding: 50px 0 37px 0;
  display: flex;
  flex-direction: column;
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  width: 100%;
}
  
.plan--premium {
  background: #0066D4;
}

.plan--premium .plan__title,
.plan--premium .plan__currency,
.plan--premium .plan__amount,
.plan--premium .plan__period,
.plan--premium .tooltip,
.plan--premium .feature-value,
.plan--premium .plan-include {
  color: #ffffff;
}

.plan--standard {
  border: 2px solid #fff;
}
  
.plan__details {
  display: flex;
  flex-direction: column;
  padding: 0px 20.67px;
}
  
.plan__title {
  color: #1c1c1c;
  letter-spacing: 0;
  margin-top: -0.86px;
  position: relative;
}
  
.plan__price {
  display: flex;
  align-items: center;
  gap: 4px;
  height: 95px;
}
  
.enterprice-plan__price {
  color: #1c1c1c;
  position: relative;
}
  
.plan__amount,
.plan__currency {
  color: #1c1c1c;
  position: relative;
}
  
.plan__period {
  color: #595959;
  position: relative;
}

.plan-button {
  padding: 30px 20.67px 0px;
  margin-bottom: 0px;
}
  
  .plan__cta {
    background: white;
    color: #0066D4;;
    width: -moz-fit-content;
    align-items: center;
    border: 1.29px solid #0066D4;
    border-radius: 3.44px;
    gap: 3.44px;
    height: 37.89px;
    justify-content: center;
    padding: 5.17px 10.33px;
    width: 100%;
  }
  
  .plan--premium .plan__cta {
    background: #0066d4;
    border-color: #ffffff; 
    color: #ffffff;
  }

  .plan__cta:hover {
    font-weight: 800;
    cursor: pointer;
  }
  
  .plan--premium .plan__cta:hover {
    font-weight: 800;
  }

  .plan__features {
    list-style: none;
    padding: 15px 20.67px 24px;
    margin: 0;
    text-align: left;
  }
  
  .plan-include {
    padding-left: 8px;
    color: #1c1c1c;
    flex: 1;
    letter-spacing: 0;
    position: relative;
    margin-top: -0.86px;
  }
  
  .plan-exclude{
    color: #595959;
  }

  .plan__feature {
    height: 28px;
    margin: 23px 0;
    padding-left: 0;
    position: relative;
    font-size: 14px;
    color: #666;
    display: flex;
    align-items: center; 
  }
  
  .plan__feature--included::before,
  .plan__feature--excluded::before {
    content: none;
  }
  
  .plan--current {
    border: 2px solid #0066D4;
    box-shadow: 0 0 10px rgba(0, 102, 212, 0.3);
  }
  
  .plan__current-label {
    color: #707070;
    text-align: center;
    margin-top: 10px;
  }
  .tooltip {
    position: relative;
    display: inline-block;
    padding-left: 8px;
    color: #1c1c1c;
  }
  
  .tooltip .tooltiptext {
    font-size: 10px; 
    font-weight: 200; 
    line-height: 1.5;
    text-align: left;
    text-underline-position: from-font;
    text-decoration-skip-ink: none;
    border-radius: 4px;
    visibility: hidden;
    width: 220px;
    background-color: #E5F1FB; 
    color: black;
    padding: 5px; 
    position: absolute;
    z-index: 1;
    bottom: 125%;
    left: 50%; 
    transform: translateX(-50%); 
    opacity: 0;
    transition: opacity 0.3s ease-in-out;
  }
  
  .tooltip .tooltiptext::after {
    content: "";
    position: absolute;
    top: 100%; 
    left: 10%; 
    transform: translateX(-50%); 
    border-width: 5px;
    border-style: solid;
    border-color: #E5F1FB transparent transparent transparent; 
  }
  
  .tooltip:hover .tooltiptext {
    visibility: visible;
    opacity: 1;
  }
  
  
  @media (max-width: 1200px) {
    .fee__plans {
      padding: 0 20px;
    }
    
    .fee__plans-grid {
      grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
      gap: 20px;
    }
    
    .plan__details {
      width: auto;
      padding: 0 20px;
    }
  }
  
  @media (max-width: 768px) {
    .fee__content {
      margin-left: 0;
    }
    
    .fee__header {
      padding: 20px;
    }
    
    .fee__billing {
      margin: 10px 15px;
    }
    
    .fee__plans {
      padding: 0 15px;
      margin: 20px 15px;
    }
  
    .plan {
      padding: 30px 20px;
    }
  }
</style>