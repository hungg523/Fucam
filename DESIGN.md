# Design System Strategy: The Radiant Canopy

## 1. Overview & Creative North Star
**Creative North Star: "The Living Editorial"**

This design system moves beyond the rigid, boxy structures of traditional education sites. It is built on the concept of **Organic Transformation**—capturing the moment a seed breaks the soil or a camper discovers a new skill. We achieve this through a "Living Editorial" approach: high-end typography scales usually reserved for luxury magazines, paired with fluid, asymmetric layouts that mimic the unpredictability of nature. 

Instead of a static landing page, the UI should feel like a curated journey. We break the "template" look by utilizing overlapping elements (images breaking out of containers) and a "Tonal Depth" strategy that replaces harsh lines with soft, environmental transitions.

---

## 2. Colors & Surface Philosophy
The palette is rooted in the "Nature Green" (`primary`) and "Energy Yellow" (`secondary/tertiary`) spectrum, but applied with a sophisticated, layered approach to avoid a "juvenile" appearance.

### The "No-Line" Rule
**Explicit Instruction:** 1px solid borders are strictly prohibited for sectioning. 
Structure must be defined through **Background Color Shifts**. For example:
- A "Curriculum" section using `surface-container-low` sits directly atop a `surface` background.
- This creates a soft "zone" rather than a hard cage, allowing the content to breathe and feel modern.

### Surface Hierarchy & Nesting
Treat the UI as physical layers of fine, organic paper.
- **Base Layer:** `surface` (#f2f8f0)
- **Content Blocks:** `surface-container` (#e3eae1)
- **Elevated Interactive Cards:** `surface-container-lowest` (#ffffff) to provide a "pop" of clean white against the nature-toned background.

### The "Glass & Gradient" Rule
To inject "soul" into the professional layout:
- **Glassmorphism:** Use for floating navigation bars or overlaying labels. Apply `surface` at 70% opacity with a `20px` backdrop-blur.
- **Signature Gradients:** For primary CTAs and Hero backgrounds, use a subtle linear gradient from `primary` (#176a21) to `primary_container` (#9df197). This mimics the way light filters through a forest canopy.

---

## 3. Typography: The Editorial Voice
We utilize two distinct personalities that work in harmony to convey "Trustworthy Professionalism" and "Vibrant Energy."

*   **Display & Headlines (Plus Jakarta Sans):** A modern, geometric sans-serif with a friendly soul. 
    *   *Usage:* Use `display-lg` (3.5rem) for hero statements. Apply a slightly tighter letter-spacing (-0.02em) to create a high-end, editorial impact.
*   **Body & Titles (Be Vietnam Pro):** Optimized for readability and a "clean" feel. 
    *   *Usage:* `body-lg` (1rem) for descriptions. Its open counters ensure that even dense information feels approachable.

**Hierarchy Note:** Always lead with a `label-md` in `secondary` uppercase before a `headline-lg` to create an "organized" academic feel within the vibrant camp context.

---

## 4. Elevation & Depth: Tonal Layering
We do not use shadows to create "fake" 3D. We use **Tonal Layering** to create "Natural Lift."

*   **The Layering Principle:** Place a `surface-container-lowest` card on a `surface-container-low` background. The slight shift in lightness creates a sophisticated separation that is felt rather than seen.
*   **Ambient Shadows:** If a floating element (like a registration modal) is required, use:
    *   `Box-shadow: 0 20px 40px rgba(42, 48, 43, 0.06);` (using the `on-surface` token at 6% opacity). This mimics soft, overcast daylight.
*   **The "Ghost Border" Fallback:** For inputs or essential containers, use the `outline_variant` (#a9afa8) at **15% opacity**. Never use 100% opacity borders.

---

## 5. Components

### Buttons (The "Leaf" Interaction)
*   **Primary:** Background: Gradient (`primary` to `primary_dim`). Text: `on_primary`. Shape: `md` (0.75rem) or `full` for a more organic feel.
*   **Secondary:** Background: `secondary_container`. Text: `on_secondary_container`. No border.
*   **Interactions:** On hover, buttons should scale 1.02x and increase the gradient intensity.

### Cards (The "Nested" Approach)
*   **Constraint:** No dividers. Use `title-lg` followed by `body-md` with a 24px (`xl`) vertical gap.
*   **Visual Element:** Every card should feature a "Transformation Element"—a subtle watermark or an icon in `tertiary_fixed_dim` that bleeds off the edge of the card.

### Input Fields
*   **Style:** `surface_container_highest` background with a `Ghost Border`.
*   **States:** Focus state uses a 2px `primary` bottom-border only, mimicking a "growth" line.

### Additional Signature Component: "The Growth Timeline"
A vertical or horizontal track using `outline_variant` at 20% opacity. The "current stage" is highlighted with a `tertiary` (Orange) dot and a glassmorphic tooltip, representing the camper’s journey of transformation.

---

## 6. Do's and Don'ts

### Do:
*   **Embrace Asymmetry:** Place a large nature photograph overlapping two different color sections to break the "grid."
*   **Use Generous White Space:** Use the spacing scale to ensure "Breathing Room" between academic sections and activity sections.
*   **Apply "Tonal Transitions":** Use the `surface-container` tiers to guide the user's eye from the most important (brightest) to least important (dimmest) content.

### Don't:
*   **No "Pure" Black:** Never use #000000. Use `on_surface` (#2a302b) for all text to maintain a natural, earthy feel.
*   **No Hard Borders:** Avoid 1px solid lines; they feel restrictive and "industrial," which contradicts the summer camp spirit.
*   **No Flat Icons:** Use dual-tone icons utilizing `primary` and `secondary_fixed_dim` to add depth and "Energy."

---

## 7. Accessibility
Ensure that all `primary` text on `surface` backgrounds maintains a 4.5:1 contrast ratio. When using `secondary` (Yellow/Orange) for labels, ensure they are paired with `on_secondary_container` or placed on a dark `primary` background to guarantee legibility for all parents and campers.