package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.List;

public class PayrollALLFULLComponentCompactViewModel {
    @JsonProperty("lstofComponentDetails")
    @JsonAlias({"lstofComponentDetails", "LstofComponentDetails"})
    private List<PayrollALLComponentCompactViewModel> lstofComponentDetails;

    @JsonProperty("lstofArrearComponentDetails")
    @JsonAlias({"lstofArrearComponentDetails", "LstofArrearComponentDetails"})
    private List<PayrollALLComponentCompactViewModel> lstofArrearComponentDetails;

    public List<PayrollALLComponentCompactViewModel> getLstofComponentDetails() { return lstofComponentDetails; }
    public void setLstofComponentDetails(List<PayrollALLComponentCompactViewModel> lstofComponentDetails) { this.lstofComponentDetails = lstofComponentDetails; }
    public List<PayrollALLComponentCompactViewModel> getLstofArrearComponentDetails() { return lstofArrearComponentDetails; }
    public void setLstofArrearComponentDetails(List<PayrollALLComponentCompactViewModel> lstofArrearComponentDetails) { this.lstofArrearComponentDetails = lstofArrearComponentDetails; }
}
