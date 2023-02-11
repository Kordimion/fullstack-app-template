import { useState } from "react";

import { withAuth, useAuth, AuthContextProps } from "oidc-react"

function App() {
  const [weather, setWeather] = useState("no weather");

  const auth : AuthContextProps = useAuth();
  const accessToken = auth.userData?.access_token;

  let userInfo = auth.userData;
  let AuthButton = (!!userInfo ? 
    <button onClick={auth.signOutRedirect}>logout</button> 
    : <button onClick={auth.signIn}>login</button>)

  let UserInfoBlock = withAuth(props => {
    return (
      <>
      <br />
      <h2 className="username">{props.userData?.profile.name}</h2>
      <br />
      <pre className="text-output">{JSON.stringify(props.userData?.scopes, null, 2)}</pre>
      </>
    )
  })


  return (<div>
    <h1>Hello world!</h1>
    <hr />
    {AuthButton}
    <hr />
    <button onClick={async () => {
      let fetchSettings : RequestInit = { method: "GET" }
      if(accessToken)
        fetchSettings.headers = {
          "Authorization": `Bearer ${accessToken}`
        }

      const res = await fetch(`/api/catalog/WeatherForecast/`, fetchSettings);
      const txt = await res.text();
      setWeather(txt);
    }}>Get Weather</button> 
    <p>Weather: {weather}</p>
    <hr />
    <UserInfoBlock />
  </div>);
}

export default App;
