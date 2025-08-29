/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    './Views/**/*.cshtml',
    './Pages/**/*.cshtml',
    './Areas/**/*.cshtml', 
    './**/*.cshtml',
    './wwwroot/**/*.js'
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}

