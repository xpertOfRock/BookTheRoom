/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
      customGamma: {
        lightGray: "#CCD9E2", 
        skyBlue: "#285185", 
        pumpkinOrange: "#D67940",
        lightCabernet: "#6f4849"
      },
    },
  },
  plugins: [],
}
}