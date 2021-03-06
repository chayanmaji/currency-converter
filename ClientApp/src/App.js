import React, {useState, useEffect } from 'react';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";


function App() {
  let currentDate = new Date();
  // currentDate = currentDate.toISOString().split('T')[0]
  //currentDate.toISOString().split('T')[0]
  const [sourceCurrency, setSourceCurreny] = useState('CAD');
  const [targetCurrency, setTargetCurrency] = useState('USD');
  const [conversionDate, setConversionDate] = useState(()=>{
    let currentDate = new Date();
    console.log('currentDate.toISOString()', currentDate.toISOString());
    console.log('currentDate.toString()', currentDate.toString());
    let formattedDate = currentDate.toISOString().substring(0,10);
    return new Date(formattedDate);
  });
  const [conversionRate, setConversionRate] = useState(null);

  const getFormattedDate = (date) => {
    console.log('date.getMonth()', date.getMonth());
    let selectedMonth = parseInt(date.getMonth()) + 1;
    let selectedDay = date.getDate();
    let year =  date.getFullYear();
    let month = selectedMonth > 9 ? `${selectedMonth}` : `0${selectedMonth}`;   //  parseInt((date.getMonth() + 1).toString(2);
    let day = selectedDay > 9 ? `${selectedDay}` : `0${selectedDay}`;
    let formattedDate = `${ year }-${ month }-${ day }`;
    console.log('formattedDate', formattedDate);
    return formattedDate;
  }

  const convertCurrency = () => {
    if (sourceCurrency !== 'CAD' && targetCurrency !== 'CAD')
    {
      alert('Atleast one currency must be CAD');
      return;
    }
    if (sourceCurrency === targetCurrency)
    {
      alert('Please select different currency.');
      return;
    }
    const requestBody = {
      sourceCurrency,
      targetCurrency,
      date : getFormattedDate(conversionDate)
    }
    fetch('/api/Exchanges/', 
          {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        })
      .then(response => response.json())
      .then(data => {
        setConversionRate( parseFloat(data).toFixed(4));
      });

  } 


  return (
    <div>
    <h1>Currency conversion</h1>
    <div><p>Source Currency</p></div>
    <div>
      <select value={sourceCurrency} onChange={(e)=>{
        setSourceCurreny(e.target.value);
      }}>
        <option value='CAD'>CAD</option>
        <option value='USD'>USD</option>
        <option value='EUR'>EUR</option>
        <option value='JPY'>JPY</option>
        <option value='GBP'>GBP</option>
        <option value='AUD'>AUD</option>
        <option value='CHF'>CHF</option>
        <option value='CNY'>CNY</option>
        <option value='HKD'>HKD</option>
        <option value='MXN'>MXN</option>
        <option value='INR'>INR</option>
      </select>
    </div>
    <div><p>Target Currency</p></div>
    <div>
    <select value={targetCurrency} onChange={(e)=>{
      console.log('target currency', e.target.value);
      setTargetCurrency(e.target.value);
    }}>
        <option value='CAD'>CAD</option>
        <option value='USD'>USD</option>
        <option value='EUR'>EUR</option>
        <option value='JPY'>JPY</option>
        <option value='GBP'>GBP</option>
        <option value='AUD'>AUD</option>
        <option value='CHF'>CHF</option>
        <option value='CNY'>CNY</option>
        <option value='HKD'>HKD</option>
        <option value='MXN'>MXN</option>
        <option value='INR'>INR</option>
      </select>
    </div>
    <div>
    <DatePicker dateFormat='yyyy-MM-dd' selected={conversionDate}  maxDate={new Date()} 
        onChange={(date) => {
                              console.log('date', date); 
                              setConversionDate(date)
                            }
                } />
    </div>
    <div>
      <button type='button' onClick={convertCurrency}>Conversion</button>
    </div>
    <div>
      <h3>Conversion Rate : {  conversionRate}</h3>
    </div>
  </div>
  );
}

export default App;
