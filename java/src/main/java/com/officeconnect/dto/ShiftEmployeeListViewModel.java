package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.List;

@JsonInclude(JsonInclude.Include.NON_NULL)
public class ShiftEmployeeListViewModel {
    @JsonProperty("ShiftEmployee")
    private List<ShiftEmployeeMasterViewModel> shiftEmployee;

    @JsonProperty("NonShiftEmployee")
    private List<ShiftEmployeeMasterViewModel> nonShiftEmployee;

    public List<ShiftEmployeeMasterViewModel> getShiftEmployee() { return shiftEmployee; }
    public void setShiftEmployee(List<ShiftEmployeeMasterViewModel> shiftEmployee) { this.shiftEmployee = shiftEmployee; }
    public List<ShiftEmployeeMasterViewModel> getNonShiftEmployee() { return nonShiftEmployee; }
    public void setNonShiftEmployee(List<ShiftEmployeeMasterViewModel> nonShiftEmployee) { this.nonShiftEmployee = nonShiftEmployee; }
}
