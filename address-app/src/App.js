import React, { useState, useEffect } from 'react';
import './App.css';

function App() {
  const [postcode, setPostcode] = useState('');
  const [address, setAddress] = useState(null);
  const [error, setError] = useState('');
  const [history, setHistory] = useState([]);
  const [distanceUnit, setDistanceUnit] = useState('km');


  const fetchAddress = async () => {
    setError('');
    setAddress(null);
    
    try {
      const response = await fetch(`http://localhost:5045/api/address/${postcode}`);
      
      if (!response.ok) {
        throw new Error('Address not found.');
      }
      
      const data = await response.json();
      setAddress(data);

      await addPostcodeToHistory(postcode);
      fetchHistory();
    } catch (err) {
      setError(err.message);
    }
  };

  const fetchHistory = async () => {
    try {
      const response = await fetch("http://localhost:5045/api/history");
      const data = await response.json();
      setHistory(data);
    } catch (error) {
      console.error("Error fetching history:", error);
    }
  };

  const addPostcodeToHistory = async (postcode) => {
    try {
      await fetch(`http://localhost:5045/api/history?postcode=${postcode}`, {
        method: "POST",
      });
    } catch (error) {
      console.error("Error adding postcode to history:", error);
    }
  };

  const getDistanceToHeathrow = () => {
    if (address) {
      const distanceKm = address.distanceToHeathrowAirportKm;
      if (distanceUnit === 'km') {
        return `${distanceKm.toFixed(2)} km`;
      } else {
        const distanceMiles = address.distanceToHeathrowAirportMiles;
        return `${distanceMiles.toFixed(2)} miles`;
      }
    }
    return null;
  };

  useEffect(() => {
    fetchHistory();
  }, []);
  

  return (
    <div className="App">
      <h1><a href="/">Address Lookup</a></h1>
      <p id="description">This app allows you to look up an address using a postcode. You can find the corresponding latitude and longitude, as well as the distance to Heathrow Airport.</p>
      <input 
        type="text" 
        value={postcode} 
        onChange={(e) => setPostcode(e.target.value)} 
        placeholder="Enter postcode" 
      />
      <button onClick={fetchAddress}>Find Address</button>
      
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {address && (
        <div>
          <h3>Address Details</h3>
          <p>{address.fullAddress}</p>
          <p>Latitude: {address.latitude}</p>
          <p>Longitude: {address.longitude}</p>
          <div>
            <h4>Distance to Heathrow Airport:</h4>
            <p>{getDistanceToHeathrow()}</p>
            <select 
              value={distanceUnit} 
              onChange={(e) => setDistanceUnit(e.target.value)}
            >
              <option value="km">Kilometers</option>
              <option value="miles">Miles</option>
            </select>
          </div>
        </div>
      )}
      <h3>Last 3 Searches</h3>
      <ul>
        {history.map((item, index) => (
          <li key={index}>{item}</li>
        ))}
      </ul>
    </div>
  );
}

export default App;
