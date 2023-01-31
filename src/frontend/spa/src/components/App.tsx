import React, { useState } from "react";

function App() {
  const [weather, setWeather] = useState("no weather");

  return (<div>
    <h1>Hello world!</h1>
    <button onClick={async () => {
      const res = await fetch("http://localhost:5142/WeatherForecast/");
      const txt = await res.text();
      setWeather(txt);
    }}>Get Weather</button> 
    <p>Weather: {weather}</p>
  </div>);
}

export default App;
