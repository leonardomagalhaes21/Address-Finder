import React, { useState } from 'react';
import './App.css';

function App() {
  const [postcode, setPostcode] = useState('');
  const [address, setAddress] = useState(null);
  const [error, setError] = useState('');

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
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="App">
      <h1>Address Lookup</h1>
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
        </div>
      )}
    </div>
  );
}

export default App;
