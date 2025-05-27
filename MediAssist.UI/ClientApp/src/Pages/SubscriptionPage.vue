<template>
  <div class="website fee">
    <div class="fee__sidebar">
      <SideBar />
    </div>
    
    <div class="fee__content">
      <header class="fee__header">
        <div class="fee__header-title">Home  
          <svg width="6" height="10" viewBox="0 0 6 10" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path fill-rule="evenodd" clip-rule="evenodd" d="M0.674613 0.269107C0.884276 0.100628 1.19993 0.123391 1.37964 0.31995L5.37964 4.69495C5.54013 4.87049 5.54013 5.12952 5.37964 5.30507L1.37964 9.68007C1.19993 9.87663 0.884276 9.89939 0.674613 9.73091C0.46495 9.56243 0.44067 9.26651 0.620381 9.06995L4.34147 5.00001L0.620381 0.930067C0.44067 0.733508 0.46495 0.437586 0.674613 0.269107Z" fill="#707070"/>
          </svg>
          Pricing
        </div>
        <div class="fee__header-actions">
          <div class="fee__user-menu">
            <img
              :src="image"
              alt="Doctor Icon"
              class="fee__user-avatar"
              @click="toggleMenu"
            />
          </div>
          <div class="fee__user-profile">
            <ProfileDropDown :menuOpen="menuOpen" />
          </div>
        </div>
      </header>
      <section class="fee-content" id="fee-content" ref="fee-content">
       <div v-if="isShowNotificationBanner" class="plan-expired-notification">
        <div class="plan-expired-notification-content">
          <img src="@/assets/Pricing/notification.svg">
          <div class="plan-expired-notification-text">
          <h4 class="notification-title h4-medium">Your trial has expired! </h4>
          <p class="lg-regular">Upgrade now to enjoy seamless access in {{store.MediAssistConfigManager.DomainName}}.</p>
          </div>
        </div>
       </div>
        <div class="fee-pricing">
          <div class="fee__billing">
            <div class="fee__header-title1">
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
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="tooltip sm-medium">
                      <span class="feature-value sm-medium">*</span>Up to<span class="feature-value">&nbsp;5</span> Transcriptions & Report Generation/month
                      <span class="tooltiptext">Pricing varies with token usage and covers up to 5 million tokens.</span>
                    </div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium">Available Access Hours<span class="feature-value">&nbsp;30</span> minutes</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium">Up to<span class="feature-value">&nbsp;10 </span>minutes session limit</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium">Real-time results</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium">AI-powered draft SOAP note generation</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium">Automatic data deletion for security compliance</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium">Seamless EHR system integration</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium">Generate documents with confidence</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Email support</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Priority access to latest models</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Early access to new AI features</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Share Reports via Email</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Watermark removal</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'free' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Tailored Capabilities & advanced support</div>
                  </div>
                </div>
                <div class="plan-button">
                  <div v-if="currentPlan !== 'free'" class="plan__cta-container">
                    <button class="plan__cta sm-semibold" @click.prevent="showScheduleDemoPopUp = true" :disabled="currentPlan !== 'free'">Contact us</button>
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
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <p class="tooltip sm-medium">
                      <span class="feature-value tooltip-trigger sm-medium">*</span>Up to<span class="feature-value">&nbsp;40</span> transcriptions per month
                      <span class="tooltiptext">Pricing varies with token usage and covers up to 5 million tokens.</span>
                    </p>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium"><span class="feature-value">10</span> hours available</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')"  alt="checkmark">
                    <div class="plan-include sm-medium">Up to<span class="feature-value">&nbsp;15</span> minutes session limit</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Real-time results</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">AI-powered draft SOAP note generation </div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Automatic data deletion for security compliance</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Seamless EHR system integration </div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Generate documents with confidence</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Email support</div>
                  </div>
                   <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Priority access to latest models</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Early access to new AI features</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Share Reports via Email</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Watermark removal</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'pro' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Tailored Capabilities & advanced support</div>
                  </div>
                </div>
                <div class="plan-button">
                  <div v-if="currentPlan !== 'pro'" class="plan__cta-container">
                    <button class="plan__cta sm-semibold" @click="ContactUs('Pro','contact-us-subscription')">Contact us</button>
                  </div>
                  <div v-else class="plan__current-label sm-semibold">
                    Current Plan
                  </div>
                </div>
              </div>

              <!-- Advanced Plan -->
              <div class="plan" :class="{ 'plan--current': currentPlan === 'advanced' }">
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
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <p class="tooltip sm-medium">
                      <span class="feature-value sm-medium">*</span>Up to<span class="feature-value">&nbsp;80</span> transcriptions per month
                      <span class="tooltiptext">Pricing varies with token usage and covers up to 5 million tokens.</span>
                    </p>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium"><span class="feature-value">20</span> hours available</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Up to<span class="feature-value">&nbsp;15</span> minutes session limit</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Real-time results</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">AI-powered draft SOAP note generation </div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Automatic data deletion for security compliance</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Seamless EHR system integration </div>
                  </div>
                   <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Generate documents with confidence</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Email with priority support</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Priority access to latest models</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Early access to new AI features</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Share Reports via Email</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Watermark removal</div>
                  </div>
                  <div class="plan__feature plan__feature--excluded">
                    <img :src="currentPlan === 'advanced' ? require('@/assets/Pricing/highlightedexclude.svg') : require('@/assets/Pricing/exclude.svg')" alt="checkmark">
                    <div class="plan-include plan-exclude sm-medium">Tailored Capabilities & advanced support</div>
                  </div>
                </div>
                <div class="plan-button">
                  <div v-if="currentPlan !== 'advanced'" class="plan__cta-container">
                    <button class="plan__cta sm-semibold"  @click="ContactUs('Advanced','contact-us-subscription')">Contact us</button>
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
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Fully customizable based on needs</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Flexible to your requirements</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Adjustable as per your preference</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Real-time results</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">AI-powered draft SOAP note generation </div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Automatic data deletion for security compliance</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Seamless EHR system integration </div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Generate documents with confidence</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">24x7 support</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Priority access to latest models</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Early access to new AI features</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Share Reports via Email</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Watermark removal</div>
                  </div>
                  <div class="plan__feature plan__feature--included">
                    <img :src="currentPlan === 'enterprise' ? require('@/assets/Pricing/highlightedinclude.svg') : require('@/assets/Pricing/include.svg')" alt="checkmark">
                    <div class="plan-include sm-medium">Tailored Capabilities & advanced support</div>
                  </div>
                </div>
                <div class="plan-button">
                  <div v-if="currentPlan !== 'enterprise'" class="plan__cta-container">
                    <button class="plan__cta sm-semibold" @click="ContactUs('Enterprise','contact-us-subscription')">Contact us</button>
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

    <ConfirmationAlert v-if="showConfirmationAlert"
      :isVisible="showConfirmationAlert"
      title="Confirm ContacUs"
      :message="confirmationAlertMessage"
      @confirm="confirmContacUs"
      @cancel="cancelContacUs" 
    />
  </div>
</template>

<script setup>
import Icon from "@/assets/doctor-icon.png";
import ProfileDropDown from '@/components/ProfileDropDown.vue';
import SideBar from '@/components/SideBar.vue';
import { useMyStore } from '@/store/store.ts';
import { computed, ref, onMounted, onUnmounted, onBeforeMount } from 'vue';
import ConfirmationAlert from "@/components/alert/ConfirmationAlert.vue";
import { useRouter } from 'vue-router';

const router = useRouter();
const menuOpen = ref(false);
const isAnnual = ref(false);
const showConfirmationAlert = ref(false);
const image = ref("");
const store = useMyStore();
const AvailableHoursExceeded = ref(false)
const GenerateTranscriptionReposrt = ref(false)
const selectedPlan = ref("");
const confirmationAlertMessage = ref("");
const selectedModalType = ref("");

const currentPlan = ref('free');

const isShowNotificationBanner = ref(false);

const proPrice = computed(() => {
  return isAnnual.value ? 22 : 24;
});

const premiumPrice = computed(() => {
  return isAnnual.value ? 43 : 48;
});

const enterprisePrice = computed(() => {
  return 'Talk to Sales';
});


const toggleMenu = () => {
  menuOpen.value = !menuOpen.value;
};

const ContactUs = (planName, modalType) => {
  showConfirmationAlert.value = true;
  selectedPlan.value = planName;
  selectedModalType.value = modalType;
  confirmationAlertMessage.value = `Are you sure you want to upgrade to the ${selectedPlan.value} plan? Your account will be charged accordingly.` 
};

const confirmContacUs = () => {
  localStorage.setItem('modalType', selectedModalType.value);
  localStorage.setItem('selectedPlan', selectedPlan.value);
  router.push('/contact-us');
};

const cancelContacUs = () => {
  showConfirmationAlert.value = false;
};

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  return null;
}

onMounted(() => {
  const imageValue = localStorage.getItem("image");

  let imageType = "png";
    if (imageValue && imageValue !== "null") {
      if (imageValue.startsWith("/9j")) {
        imageType = "jpeg";
      }
      image.value = `data:image/${imageType};base64,${imageValue}`;
    }
    else{
      image.value = Icon;
    }

  document.addEventListener("click", handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener("click", handleClickOutside);
});
const handleClickOutside = (event) => {
  const menu = document.querySelector(".fee__user-profile");
  const userIcon = document.querySelector(".fee__user-avatar");
  if (
    menu &&
    !menu.contains(event.target) &&
    !userIcon.contains(event.target)
  ) {
    menuOpen.value = false;
  }
};

onBeforeMount(async () => {
  let userId = getCookie("userId");
  await store.fetchUserActivityMetrics(userId);
  AvailableHoursExceeded.value = store.UserActivityMetrics.AvailableHours <= 0 ;
  GenerateTranscriptionReposrt.value = store.UserActivityMetrics.Transcriptions > 0 ;

  if (!GenerateTranscriptionReposrt.value || AvailableHoursExceeded.value) {
    isShowNotificationBanner.value = true;
  }
  else{
    isShowNotificationBanner.value = false;
  }
});


const updatePrices = () => {
  // Price update logic if needed
};
</script>

<style scoped>
.fee {
  height: 100vh;
  background-color: #f5f7fb;
  overflow-y: auto;
  position: relative;
}

.fee__sidebar {
  width: 80px;
  position: fixed;
  height: 100vh;
  background-color: #1a1f36;
}

.fee__content {
  flex: 1;
  margin-left: 80px;
}

.fee-content{
  align-items: flex-start;
  background-color: #f7fbff;
  display: flex;
  flex-direction: column;
  gap: 24px;
  padding: 40px 80px 80px;
  position: relative;
}

.notification-title{
  margin: 0px;
}

.title-text{
  align-self: stretch;
  color: #000000;
  font-family: "Inter-Medium", Helvetica;
  font-size: 24px;
  font-weight: 500;
  letter-spacing: 0;
  line-height: normal;
  margin-top: -1.00px;
  position: relative;
  margin: 0px;
}
 
.subtitle-text{
  align-self: stretch;
  color: #000000;
  font-family: "Inter-Regular", Helvetica;
  font-size: 16px;
  font-weight: 400;
  letter-spacing: 0;
  line-height: 24px;
  position: relative;
  margin: 0px;
}

.fee-pricing{
  align-items: center;
  border-radius: 3.44px;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  position: relative;
  width: 100%;
}

.fee__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: white;
  padding: 29px;
  border-bottom: 1px solid #e5e7eb;
  position: sticky;
  top: 0;
  z-index: 1000;
}

.fee__header-title {
  font-size: 14px;
  font-weight: 400;
  line-height: 21px;
  text-align: left;
  text-underline-position: from-font;
  text-decoration-skip-ink: none;
  color: #1C1C1C;
}

.fee__header-title h1{
  margin: 0px;
}

.fee__header-actions {
  display: flex;
  align-items: center;
  gap: 20px;
}

.fee__user-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  object-fit: cover;
  cursor: pointer;
}

.fee__billing {
  align-items: center;
  display: flex;
  gap: 660px;
  justify-content: center;
  position: relative;
  margin: 10px 15px;
}

.fee__header-title1{
  align-items: flex-start;
  display: flex;
  align-self: stretch;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  justify-content: center;
  position: relative;
}

.pricing-title{
  letter-spacing: 0;
  position: relative;
  white-space: nowrap;
  width: fit-content;
}

.plan-expired-notification{
    padding: 20px;
   
    display: grid;
    flex-direction: row;
    align-items: center;
    max-width: 1240px;
    grid-template-columns: 3fr 1fr;
    width: 100%;
    margin-inline: auto;
    border-radius: 16px;
    border: 1px solid #EC9112;
    background: #FFFBF3;
}

.plan-expired-notification p{
  color: var(--Text-Primary, #1C1C1C);
  margin: 0px;
}
.plan-expired-notification h3{
  margin:  0px;
  color: var(--Text-Primary, #1C1C1C);
}
.plan-expired-notification-content{
  display: flex;
  align-items: center;
  flex: 1;
  flex-grow: 1;
  gap: 16px;
}
.plan-expired-notification-text{
  display: flex;
  align-items: flex-start;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
}

.fee-frame{
  align-items: center;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 8px;
  position: relative;
  width: 100%;
}

.fee-frame1{
  align-items: center;
  display: flex;
  flex: 1;
  flex-grow: 1;
  gap: 16px;
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
  font-family: "Inter-medium", Helvetica;
  font-size: 14px;
  font-weight: 500;
  letter-spacing: 0;
  line-height: 21px;
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
  /* background: linear-gradient(0deg, #2d4f9f, #2d4f9f), linear-gradient(274.4deg, #0057b4 2.36%, #00254d 97.81%); */
}

.fee__switch-input:checked + .fee__switch-slider {
  transform: translateX(100px);
}

.fee__plans {
  margin: 30px auto;
  width: 100%;
  margin-bottom: 0px;
}

.fee__plans-grid {
  display: grid;
  grid-template-columns: repeat(4, minmax(300px, 405px));
  gap: 25px;
  justify-content: center;
  margin-inline: auto;
    /* width: 100%; */
  max-width: 1240px;
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

.plan__details {
  display: flex;
  flex-direction: column;
  padding: 0px 20.67px;
}

.plan__title {
  color: #1c1c1c;
  letter-spacing: 0;
  line-height: normal;
  margin: 0px;
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

.plan__cta {
  background: white;
  color: #0066d4;
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

.plan__cta:hover {
  font-weight: 800;
  cursor: pointer;
}

.plan__features {
  list-style: none;
  padding: 15px 20.67px 24px;
  margin: 0;
  text-align: left;
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

.plan-include {
  padding-left: 8px;
  color: #1c1c1c;
  letter-spacing: 0;
  line-height: 21px;
  position: relative;
  margin-top: -0.86px;
  flex: 1;
}

.plan-exclude{
  color: #595959;
}

.plan-button {
  padding: 30px 20.67px 0px;
  margin-bottom: 0px;
}
.plan--current {
  background: #0066d4; 
  /* background: linear-gradient(89.87deg, #104582 0.11%, #052A56 55.33%); */
}

.plan--current .plan__title,
.plan--current .plan__currency,
.plan--current .plan__amount,
.plan--current .plan__period,
.plan--current .tooltip,
.plan--current .feature-value,
.plan--current .plan-include,
.plan--current .enterprice-plan__price {
  color: #ffffff;
}

.plan--current .plan__current-label {
  align-items: center;
  background-color: #ffffff4c;
  border-radius: 20px;
  display: flex;
  gap: 3.44px;
  height: 37.89px;
  justify-content: center;
  position: relative;
  color: #ffffff;
  text-align: center;
}

.tooltip {
  position: relative;
  display: inline-block;
  padding-left: 8px;
  color: #1c1c1c;
}

.tooltip .tooltiptext {
  font-family: Lato, sans-serif;
  font-size: 10px; 
  font-weight: 200; 
  line-height: 1.5;
  text-underline-position: from-font;
  text-decoration-skip-ink: none;
  border-radius: 4px;
  visibility: hidden;
  width: 220px;
  background-color: #E5F1FB; 
  color: black;
  text-align: center;
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

.fee__user-menu{
  position: fixed;
  right: 46px;
  top: 22px;
}

button:disabled {
    background: #F6F6F6;
    color: #7E7E7E;
    cursor: not-allowed;
    opacity: 0.7;
    border: none;
}

button:disabled:hover {
    background: #F6F6F6;
    color: #7E7E7E;
    cursor: not-allowed;
    opacity: 0.7;
    border: none;
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
  
  .fee__plans {
    padding: 0 15px;
    margin: 20px 15px;
  }

  .plan {
    padding: 30px 20px;
  }
}
</style>