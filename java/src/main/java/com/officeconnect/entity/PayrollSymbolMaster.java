package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "PayrollSymbolMaster")
public class PayrollSymbolMaster {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "SymbolId")
    private Integer symbolId;

    @Column(name = "Symbol")
    private String symbol;

    @Column(name = "IsActive")
    private Boolean isActive;

    public Integer getSymbolId() { return symbolId; }
    public void setSymbolId(Integer symbolId) { this.symbolId = symbolId; }

    public String getSymbol() { return symbol; }
    public void setSymbol(String symbol) { this.symbol = symbol; }

    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }
}
